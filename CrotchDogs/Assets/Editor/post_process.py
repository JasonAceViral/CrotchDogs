import os
from sys import argv
from mod_pbxproj import XcodeProject
import appcontroller

path = argv[1]
fileToAddPath = argv[2]


print('----------------------------------prepare for excuting our magic scripts to tweak our xcode ----------------------------------')
#path: /Users/tuo/UnityWorkspace/XCode/PigRush-XCode-Test1
print('post_process.py xcode build path --> ' + path)
print('post_process.py third party files path --> ' + fileToAddPath)

#add Facebook PList items
urlTypes = '''
	<key>FacebookAppID</key>
	<string>109014809249097</string>
	<key>CFBundleURLTypes</key>
		<array>
			<dict>
				<key>CFBundleURLName</key>
				<string></string>
				<key>CFBundleURLSchemes</key>
				<array>
					<string>fb109014809249097</string>
				</array>
			</dict>
		</array>
	</dict>
</plist>
'''

print(urlTypes)

class front_appender:
    def __init__(self, fname, mode='w'):
        self.__f = open(fname, mode)
        self.__write_queue = []

    def write(self, s):
        self.__write_queue.insert(0, s)

    def close(self):
        self.__f.writelines(self.__write_queue)
        self.__f.close()

f = open(path+'/Info.plist','r+')
content = f.read()
if 'fbXXXXX' in content:
    print "*******************Append Action, skip custom script"
    sys.exit("quit exeuction")

print "*******************Replace Action, executing our script"
f.seek(content.find('</dict>\n</plist>'))
f.write(urlTypes)
f.close()

     
    
print('Step 1: start add libraries ')
project = XcodeProject.Load(path +'/Unity-iPhone.xcodeproj/project.pbxproj')
project.add_file('System/Library/Frameworks/Accounts.framework', tree='SDKROOT', weak=True)
project.add_file('System/Library/Frameworks/AdSupport.framework', tree='SDKROOT', weak=True)
project.add_file('System/Library/Frameworks/MessageUI.framework', tree='SDKROOT')
project.add_file('System/Library/Frameworks/StoreKit.framework', tree='SDKROOT')
project.add_file('System/Library/Frameworks/Social.framework', tree='SDKROOT', weak=True)
project.add_file('System/Library/Frameworks/Twitter.framework', tree='SDKROOT', weak=True)
project.add_file('usr/lib/libsqlite3.dylib', tree='SDKROOT')

print('Step 2: add files to xcode, exclude any .meta file')
files_in_dir = os.listdir(fileToAddPath)
for f in files_in_dir:
    if not f.startswith('.'): #ignore .DS_STORE
	pathname = os.path.join(fileToAddPath, f)
	fileName, fileExtension = os.path.splitext(pathname)
	if fileExtension == '.framework': #add any new frameworks as weak references
	    print "Adding framework : " + pathname
	    project.add_file(pathname, weak=True)
	else:
	    if not fileExtension == '.meta': #ignore .meta as it is under asset server
		if os.path.isfile(pathname):
		    print "is file : " + pathname
		    project.add_file(pathname)
		if os.path.isdir(pathname):
		    print "is dir : " + pathname
		    project.add_folder(pathname, excludes=["^.*\.meta$"])

print('Step 3: modify the AppController')
appcontroller.touch_implementation(path + '/Classes/UnityAppController.mm')

print('Step 4: change build setting')
project.add_other_buildsetting('GCC_ENABLE_OBJC_EXCEPTIONS', 'YES')
project.add_other_ldflags('-lsqlite3.0')
project.add_other_ldflags('-ObjC')
project.add_other_ldflags('-all_load')
project.add_library_search_paths('$(SRCROOT)/' + os.path.relpath(fileToAddPath, path))

if project.modified:
  project.backup()
  project.saveFormat3_2()

print('----------------------------------end for excuting our magic scripts to tweak our xcode ----------------------------------')